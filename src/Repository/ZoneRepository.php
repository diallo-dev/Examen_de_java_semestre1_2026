<?php

namespace App\Repository;

use App\Entity\Zone;
use Doctrine\Bundle\DoctrineBundle\Repository\ServiceEntityRepository;
use Doctrine\Persistence\ManagerRegistry;


class ZoneRepository extends ServiceEntityRepository
{
    public function __construct(ManagerRegistry $registry)
    {
        parent::__construct($registry, Zone::class);
    }

    
    public function findAllWithCommandesCount(): array
    {
        $conn = $this->getEntityManager()->getConnection();
        
        
        $sql = "SELECT 
                    z.id,
                    z.prix_livraison,
                    COUNT(c.id) as nb_commandes
                FROM zone z
                LEFT JOIN commande c ON c.id_zone = z.id 
                    AND (LOWER(c.lieu_consommation) = 'livraison')
                    AND c.id_livreur IS NULL
                    AND c.etat_cmd = 'NonTraiter'
                GROUP BY z.id, z.prix_livraison
                ORDER BY z.id";
        
        $stmt = $conn->prepare($sql);
        $result = $stmt->executeQuery();
        
        return $result->fetchAllAssociative();
    }

    
    public function getQuartiersByZone(int $zoneId): array
    {
        $conn = $this->getEntityManager()->getConnection();
        
        $sql = "SELECT nom FROM quartier WHERE id_zone = :zoneId ORDER BY nom ASC";
        
        $stmt = $conn->prepare($sql);
        $stmt->bindValue('zoneId', $zoneId);
        $result = $stmt->executeQuery();
        
        return $result->fetchAllAssociative();
    }
}