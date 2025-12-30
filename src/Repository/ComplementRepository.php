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
