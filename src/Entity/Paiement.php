<?php
namespace App\Entity;
use Doctrine\ORM\Mapping as ORM;

#[ORM\Entity]
#[ORM\Table(name: 'paiement')]
class Paiement
{
    #[ORM\Id, ORM\GeneratedValue, ORM\Column]
    private ?int $id = null;
    #[ORM\Column(type: 'date', nullable: true)]
    private ?\DateTimeInterface $date = null;
    #[ORM\Column(nullable: true)]
    private ?float $montant = null;
    #[ORM\Column(length: 255, nullable: true)]
    private ?string $mode = null;
    #[ORM\Column(name: 'id_commande', nullable: true)]
    private ?int $idCommande = null;

    public function getId(): ?int { return $this->id; }
    public function getDate(): ?\DateTimeInterface { return $this->date; }
    public function getMontant(): ?float { return $this->montant; }
    public function getMode(): ?string { return $this->mode; }
    public function getIdCommande(): ?int { return $this->idCommande; }
}
