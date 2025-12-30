<?php
namespace App\Entity;
use Doctrine\ORM\Mapping as ORM;

#[ORM\Entity]
#[ORM\Table(name: 'zone')]
class Zone
{
    #[ORM\Id, ORM\GeneratedValue, ORM\Column]
    private ?int $id = null;
    #[ORM\Column(name: 'prix_livraison', nullable: true)]
    private ?float $prixLivraison = null;

    public function getId(): ?int { return $this->id; }
    public function getPrixLivraison(): ?float { return $this->prixLivraison; }
}
