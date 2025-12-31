<?php

namespace App\Repository;

use App\Entity\Livreur;
use Doctrine\Bundle\DoctrineBundle\Repository\ServiceEntityRepository;
use Doctrine\Persistence\ManagerRegistry;

class LivreurRepository extends ServiceEntityRepository
{
    public function __construct(ManagerRegistry $registry)
    {
        parent::__construct($registry, Livreur::class);
    }

    
    public function findAllActifs(): array
    {
        return $this->createQueryBuilder('l')
            ->orderBy('l.prenom', 'ASC')
            ->addOrderBy('l.nom', 'ASC')
            ->getQuery()
            ->getResult();
    }

    
    public function findDisponibles(): array
    {
        $conn = $this->getEntityManager()->getConnection();
        
        $sql = "SELECT l.* 
                FROM livreur l
                WHERE l.id_livreur NOT IN (
                    SELECT DISTINCT c.id_livreur 
                    FROM commande c 
                    WHERE c.id_livreur IS NOT NULL 
                    AND c.etat_cmd = 'NonTraiter'
                )
                ORDER BY l.prenom, l.nom";
        
        $stmt = $conn->prepare($sql);
        $result = $stmt->executeQuery();
        
        return $result->fetchAllAssociative();
    }

    
    public function countCommandesEnCours(int $livreurId): int
    {
        $conn = $this->getEntityManager()->getConnection();
        
        $result = $conn->executeQuery(
            "SELECT COUNT(*) as nb 
             FROM commande 
             WHERE id_livreur = :livreurId 
             AND etat_cmd = 'NonTraiter'",
            ['livreurId' => $livreurId]
        )->fetchAssociative();
        
        return (int) ($result['nb'] ?? 0);
    }
}
