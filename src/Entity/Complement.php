<?php
namespace App\Entity;
use App\Repository\ComplementRepository;
use Doctrine\ORM\Mapping as ORM;

#[ORM\Entity(repositoryClass: ComplementRepository::class)]
#[ORM\Table(name: 'complement')]
class Complement
{
    #[ORM\Id, ORM\GeneratedValue, ORM\Column]
    private ?int $id = null;
    #[ORM\Column(length: 255, nullable: true)]
    private ?string $nom = null;
    #[ORM\Column(nullable: true)]
    private ?float $prix = null;
    #[ORM\Column(name: 'url_image', length: 255, nullable: true)]
    private ?string $urlImage = null;
    #[ORM\Column(length: 255, nullable: true)]
    private ?string $etatstock = 'disponible';

    public function getId(): ?int { return $this->id; }
    public function getNom(): ?string { return $this->nom; }
    public function setNom(?string $nom): static { $this->nom = $nom; return $this; }
    public function getPrix(): ?float { return $this->prix; }
    public function setPrix(?float $prix): static { $this->prix = $prix; return $this; }
    public function getUrlImage(): ?string { return $this->urlImage; }
    public function setUrlImage(?string $urlImage): static { $this->urlImage = $urlImage; return $this; }
    public function getEtatstock(): ?string { return $this->etatstock; }
    public function setEtatstock(?string $etatstock): static { $this->etatstock = $etatstock; return $this; }
    public function isArchive(): bool { return $this->etatstock === 'archiver'; }
}
