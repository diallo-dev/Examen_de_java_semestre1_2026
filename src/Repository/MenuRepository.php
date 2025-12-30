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
}
