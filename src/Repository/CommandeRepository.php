<?php

namespace App\Repository;

use App\Entity\Commande;
use Doctrine\Bundle\DoctrineBundle\Repository\ServiceEntityRepository;
use Doctrine\Persistence\ManagerRegistry;

class CommandeRepository extends ServiceEntityRepository
{
    public function __construct(ManagerRegistry $registry)
    {
        parent::__construct($registry, Commande::class);
    }

    public function findAllWithDetails(): array
    {
        return $this->createQueryBuilder('c')
            ->leftJoin('c.client', 'cl')
            ->leftJoin('c.zone', 'z')
            ->leftJoin('c.livreur', 'l')
            ->addSelect('cl', 'z', 'l')
            ->orderBy('c.date', 'DESC')
            ->addOrderBy('c.id', 'DESC')
            ->getQuery()
            ->getResult();
    }

    /**
     * Correction du filtre : accepte string ou int pour $clientId
     */
    public function findByFilters(?string $etat = null, ?string $date = null, $clientId = null): array
    {
        $qb = $this->createQueryBuilder('c')
            ->leftJoin('c.client', 'cl')
            ->addSelect('cl');

        if ($etat && $etat !== '') {
            $qb->andWhere('c.etatCmd = :etat')
               ->setParameter('etat', $etat);
        }

        if ($date && $date !== '') {
            try {
                $qb->andWhere('c.date = :date')
                   ->setParameter('date', new \DateTime($date));
            } catch (\Exception $e) {
                // Si la date est invalide, on ignore ce filtre
            }
        }

        if ($clientId !== null && $clientId !== '') {
            $qb->andWhere('c.client = :clientId')
               ->setParameter('clientId', $clientId);
        }

        return $qb->orderBy('c.date', 'DESC')
                  ->addOrderBy('c.id', 'DESC')
                  ->getQuery()
                  ->getResult();
    }

    /**
     * Version optimisée pour l'affichage dans la zone de livraison
     */
    public function findByZone(int $zoneId): array
    {
        return $this->createQueryBuilder('c')
            ->leftJoin('c.client', 'cl') // Jointure indispensable pour afficher nom/adresse
            ->addSelect('cl')
            ->where('c.zone = :zoneId')
            ->andWhere('c.livreur IS NULL') // On ne veut que les commandes sans livreur
            ->andWhere('LOWER(c.lieuConsommation) = LOWER(:livraison)') // Flexible sur Livraison/livraison
            ->setParameter('zoneId', $zoneId)
            ->setParameter('livraison', 'Livraison')
            ->orderBy('c.date', 'DESC')
            ->getQuery()
            ->getResult();
    }

    public function getItemsByCommandeId(int $commandeId): array
    {
        $conn = $this->getEntityManager()->getConnection();
        
        $burgers = $conn->executeQuery(
            'SELECT b.nom, b.url_image, cb.quantite, cb.prix_unitaire, 
                    (cb.quantite * cb.prix_unitaire) as total
             FROM commande_burger cb
             INNER JOIN burger b ON cb.id_burger = b.id
             WHERE cb.id_commande = ?',
            [$commandeId]
        )->fetchAllAssociative();
        
        $menus = $conn->executeQuery(
            'SELECT m.nom, m.url_image, cm.quantite, cm.prix_unitaire,
                    (cm.quantite * cm.prix_unitaire) as total
             FROM commande_menu cm
             INNER JOIN menu m ON cm.id_menu = m.id
             WHERE cm.id_commande = ?',
            [$commandeId]
        )->fetchAllAssociative();
        
        return array_merge($burgers, $menus);
    }

    public function annulerCommande(int $id): void
    {
        $this->createQueryBuilder('c')
            ->update()
            ->set('c.etatCmd', ':etat')
            ->where('c.id = :id')
            ->setParameter('etat', 'Annuler')
            ->setParameter('id', $id)
            ->getQuery()
            ->execute();
    }

    public function terminerCommande(int $id): void
    {
        $this->createQueryBuilder('c')
            ->update()
            ->set('c.etatCmd', ':etat')
            ->where('c.id = :id')
            ->setParameter('etat', 'Terminer')
            ->setParameter('id', $id)
            ->getQuery()
            ->execute();
    }

            public function affecterLivreur(int $commandeId, int $livreurId): void
        {
            $conn = $this->getEntityManager()->getConnection();
            
            // Requête SQL directe pour mettre à jour le livreur et l'état
            $sql = "UPDATE commande 
                    SET id_livreur = :livreurId, 
                        etat_cmd = 'EnCours' 
                    WHERE id = :commandeId";
            
            $conn->executeStatement($sql, [
                'livreurId' => $livreurId,
                'commandeId' => $commandeId
            ]);
        }
  
}