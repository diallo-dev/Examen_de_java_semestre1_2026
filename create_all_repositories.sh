#!/bin/bash

cd /home/claude/symfony-brasil-burger-complet

# BurgerRepository
cat > src/Repository/BurgerRepository.php << 'EOFPHP'
<?php
namespace App\Repository;

use App\Entity\Burger;
use Doctrine\Bundle\DoctrineBundle\Repository\ServiceEntityRepository;
use Doctrine\Persistence\ManagerRegistry;

class BurgerRepository extends ServiceEntityRepository
{
    public function __construct(ManagerRegistry $registry)
    {
        parent::__construct($registry, Burger::class);
    }

    public function findAllNonArchive(): array
    {
        return $this->createQueryBuilder('b')
            ->where('b.etatstock != :archive')
            ->setParameter('archive', 'archiver')
            ->orderBy('b.nom', 'ASC')
            ->getQuery()
            ->getResult();
    }

    public function findByEtatstock(string $etat): array
    {
        return $this->createQueryBuilder('b')
            ->where('b.etatstock = :etat')
            ->setParameter('etat', $etat)
            ->orderBy('b.nom', 'ASC')
            ->getQuery()
            ->getResult();
    }

    public function archiverBurger(int $id): void
    {
        $this->createQueryBuilder('b')
            ->update()
            ->set('b.etatstock', ':etat')
            ->where('b.id = :id')
            ->setParameter('etat', 'archiver')
            ->setParameter('id', $id)
            ->getQuery()
            ->execute();
    }
}
EOFPHP

# MenuRepository
cat > src/Repository/MenuRepository.php << 'EOFPHP'
<?php
namespace App\Repository;

use App\Entity\Menu;
use Doctrine\Bundle\DoctrineBundle\Repository\ServiceEntityRepository;
use Doctrine\Persistence\ManagerRegistry;

class MenuRepository extends ServiceEntityRepository
{
    public function __construct(ManagerRegistry $registry)
    {
        parent::__construct($registry, Menu::class);
    }

    public function findAllNonArchive(): array
    {
        return $this->createQueryBuilder('m')
            ->where('m.etatstock != :archive')
            ->setParameter('archive', 'archiver')
            ->orderBy('m.nom', 'ASC')
            ->getQuery()
            ->getResult();
    }

    public function findByEtatstock(string $etat): array
    {
        return $this->createQueryBuilder('m')
            ->where('m.etatstock = :etat')
            ->setParameter('etat', $etat)
            ->orderBy('m.nom', 'ASC')
            ->getQuery()
            ->getResult();
    }

    public function archiverMenu(int $id): void
    {
        $this->createQueryBuilder('m')
            ->update()
            ->set('m.etatstock', ':etat')
            ->where('m.id = :id')
            ->setParameter('etat', 'archiver')
            ->setParameter('id', $id)
            ->getQuery()
            ->execute();
    }

    public function getBurgerByMenuId(int $menuId): ?array
    {
        $conn = $this->getEntityManager()->getConnection();
        $sql = 'SELECT b.* FROM burger b
                INNER JOIN menu_burger mb ON b.id = mb.id_burger
                WHERE mb.id_menu = :menuId LIMIT 1';
        $stmt = $conn->prepare($sql);
        $result = $stmt->executeQuery(['menuId' => $menuId]);
        return $result->fetchAssociative() ?: null;
    }

    public function getComplementsByMenuId(int $menuId): array
    {
        $conn = $this->getEntityManager()->getConnection();
        $sql = 'SELECT c.* FROM complement c
                INNER JOIN menu_complement mc ON c.id = mc.id_complement
                WHERE mc.id_menu = :menuId
                ORDER BY mc.id';
        $stmt = $conn->prepare($sql);
        $result = $stmt->executeQuery(['menuId' => $menuId]);
        return $result->fetchAllAssociative();
    }
}
EOFPHP

# ComplementRepository
cat > src/Repository/ComplementRepository.php << 'EOFPHP'
<?php
namespace App\Repository;

use App\Entity\Complement;
use Doctrine\Bundle\DoctrineBundle\Repository\ServiceEntityRepository;
use Doctrine\Persistence\ManagerRegistry;

class ComplementRepository extends ServiceEntityRepository
{
    public function __construct(ManagerRegistry $registry)
    {
        parent::__construct($registry, Complement::class);
    }

    public function findAllNonArchive(): array
    {
        return $this->createQueryBuilder('c')
            ->where('c.etatstock != :archive')
            ->setParameter('archive', 'archiver')
            ->orderBy('c.nom', 'ASC')
            ->getQuery()
            ->getResult();
    }

    public function archiverComplement(int $id): void
    {
        $this->createQueryBuilder('c')
            ->update()
            ->set('c.etatstock', ':etat')
            ->where('c.id = :id')
            ->setParameter('etat', 'archiver')
            ->setParameter('id', $id)
            ->getQuery()
            ->execute();
    }
}
EOFPHP

# CommandeRepository
cat > src/Repository/CommandeRepository.php << 'EOFPHP'
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
EOFPHP

# StatistiqueRepository
cat > src/Repository/StatistiqueRepository.php << 'EOFPHP'
<?php
namespace App\Repository;

use Doctrine\Bundle\DoctrineBundle\Repository\ServiceEntityRepository;
use Doctrine\Persistence\ManagerRegistry;
use App\Entity\Commande;

class StatistiqueRepository extends ServiceEntityRepository
{
    public function __construct(ManagerRegistry $registry)
    {
        parent::__construct($registry, Commande::class);
    }

    public function getCommandesEnCoursDuJour(): int
    {
        return (int) $this->createQueryBuilder('c')
            ->select('COUNT(c.id)')
            ->where('c.date = :today')
            ->andWhere('c.etatCmd = :etat')
            ->setParameter('today', new \DateTime('today'))
            ->setParameter('etat', 'NonTraiter')
            ->getQuery()
            ->getSingleScalarResult();
    }

    public function getCommandesValidesDuJour(): int
    {
        return (int) $this->createQueryBuilder('c')
            ->select('COUNT(c.id)')
            ->where('c.date = :today')
            ->andWhere('c.etatCmd = :etat')
            ->setParameter('today', new \DateTime('today'))
            ->setParameter('etat', 'Terminer')
            ->getQuery()
            ->getSingleScalarResult();
    }

    public function getCommandesAnnuleesDuJour(): int
    {
        return (int) $this->createQueryBuilder('c')
            ->select('COUNT(c.id)')
            ->where('c.date = :today')
            ->andWhere('c.etatCmd = :etat')
            ->setParameter('today', new \DateTime('today'))
            ->setParameter('etat', 'Annuler')
            ->getQuery()
            ->getSingleScalarResult();
    }

    public function getRecettesJournalieres(): float
    {
        $result = $this->createQueryBuilder('c')
            ->select('SUM(c.montantTotal)')
            ->where('c.date = :today')
            ->andWhere('c.etatCmd != :annuler')
            ->setParameter('today', new \DateTime('today'))
            ->setParameter('annuler', 'Annuler')
            ->getQuery()
            ->getSingleScalarResult();
        
        return (float) ($result ?? 0);
    }

    public function getBurgersPlusVendusDuJour(int $limit = 5): array
    {
        $conn = $this->getEntityManager()->getConnection();
        $sql = 'SELECT b.nom, b.url_image, SUM(cb.quantite) as total_vendu
                FROM commande_burger cb
                INNER JOIN burger b ON cb.id_burger = b.id
                INNER JOIN commande c ON cb.id_commande = c.id
                WHERE c.date = CURRENT_DATE AND c.etat_cmd != :annuler
                GROUP BY b.id, b.nom, b.url_image
                ORDER BY total_vendu DESC
                LIMIT :limit';
        
        $stmt = $conn->prepare($sql);
        $result = $stmt->executeQuery([
            'annuler' => 'Annuler',
            'limit' => $limit
        ]);
        
        return $result->fetchAllAssociative();
    }

    public function getMenusPlusVendusDuJour(int $limit = 5): array
    {
        $conn = $this->getEntityManager()->getConnection();
        $sql = 'SELECT m.nom, m.url_image, SUM(cm.quantite) as total_vendu
                FROM commande_menu cm
                INNER JOIN menu m ON cm.id_menu = m.id
                INNER JOIN commande c ON cm.id_commande = c.id
                WHERE c.date = CURRENT_DATE AND c.etat_cmd != :annuler
                GROUP BY m.id, m.nom, m.url_image
                ORDER BY total_vendu DESC
                LIMIT :limit';
        
        $stmt = $conn->prepare($sql);
        $result = $stmt->executeQuery([
            'annuler' => 'Annuler',
            'limit' => $limit
        ]);
        
        return $result->fetchAllAssociative();
    }
}
EOFPHP

# GestionnaireRepository
cat > src/Repository/GestionnaireRepository.php << 'EOFPHP'
<?php
namespace App\Repository;

use App\Entity\Gestionnaire;
use Doctrine\Bundle\DoctrineBundle\Repository\ServiceEntityRepository;
use Doctrine\Persistence\ManagerRegistry;
use Symfony\Component\Security\Core\Exception\UnsupportedUserException;
use Symfony\Component\Security\Core\User\PasswordAuthenticatedUserInterface;
use Symfony\Component\Security\Core\User\PasswordUpgraderInterface;

class GestionnaireRepository extends ServiceEntityRepository implements PasswordUpgraderInterface
{
    public function __construct(ManagerRegistry $registry)
    {
        parent::__construct($registry, Gestionnaire::class);
    }

    public function upgradePassword(PasswordAuthenticatedUserInterface $user, string $newHashedPassword): void
    {
        if (!$user instanceof Gestionnaire) {
            throw new UnsupportedUserException(sprintf('Instances of "%s" are not supported.', get_class($user)));
        }

        $user->setPassword($newHashedPassword);
        $this->getEntityManager()->persist($user);
        $this->getEntityManager()->flush();
    }

    public function findByMatricule(string $matricule): ?Gestionnaire
    {
        return $this->createQueryBuilder('g')
            ->where('g.matricule = :matricule')
            ->setParameter('matricule', $matricule)
            ->getQuery()
            ->getOneOrNullResult();
    }
}
EOFPHP

echo "✅ Tous les repositories créés"
