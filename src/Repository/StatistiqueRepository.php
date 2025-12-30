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
