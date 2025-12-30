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
