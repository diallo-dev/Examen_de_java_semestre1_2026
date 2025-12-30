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

    public function findByFilters(?string $etat = null, ?string $date = null, ?int $clientId = null): array
    {
        $qb = $this->createQueryBuilder('c')
            ->leftJoin('c.client', 'cl')
            ->addSelect('cl');

        if ($etat) {
            $qb->andWhere('c.etatCmd = :etat')
               ->setParameter('etat', $etat);
        }

        if ($date) {
            $qb->andWhere('c.date = :date')
               ->setParameter('date', new \DateTime($date));
        }

        if ($clientId) {
            $qb->andWhere('c.idClient = :clientId')
               ->setParameter('clientId', $clientId);
        }

        return $qb->orderBy('c.date', 'DESC')
                  ->addOrderBy('c.id', 'DESC')
                  ->getQuery()
                  ->getResult();
    }

    public function findByZone(int $zoneId): array
    {
        return $this->createQueryBuilder('c')
            ->where('c.idZone = :zoneId')
            ->andWhere('c.lieuConsommation = :livraison')
            ->andWhere('c.idLivreur IS NULL')
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
             WHERE cb.id_commande = :commandeId',
            ['commandeId' => $commandeId]
        )->fetchAllAssociative();
        
        $menus = $conn->executeQuery(
            'SELECT m.nom, m.url_image, cm.quantite, cm.prix_unitaire,
                    (cm.quantite * cm.prix_unitaire) as total
             FROM commande_menu cm
             INNER JOIN menu m ON cm.id_menu = m.id
             WHERE cm.id_commande = :commandeId',
            ['commandeId' => $commandeId]
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
        $this->createQueryBuilder('c')
            ->update()
            ->set('c.idLivreur', ':livreurId')
            ->where('c.id = :id')
            ->setParameter('livreurId', $livreurId)
            ->setParameter('id', $commandeId)
            ->getQuery()
            ->execute();
    }
}
