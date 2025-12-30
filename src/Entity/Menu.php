<?php
namespace App\Entity;
use App\Repository\MenuRepository;
use Doctrine\ORM\Mapping as ORM;

#[ORM\Entity(repositoryClass: MenuRepository::class)]
#[ORM\Table(name: 'menu')]
class Menu
{
    #[ORM\Id, ORM\GeneratedValue, ORM\Column]
    private ?int $id = null;
    #[ORM\Column(length: 255, nullable: true)]
    private ?string $nom = null;
    #[ORM\Column(name: 'url_image', length: 255, nullable: true)]
    private ?string $urlImage = null;
    #[ORM\Column(type: 'text', nullable: true)]
    private ?string $description = null;
    #[ORM\Column(name: 'prix_total', nullable: true)]
    private ?float $prixTotal = null;
    #[ORM\Column(length: 255, nullable: true)]
    private ?string $etatstock = 'disponible';

    public function getId(): ?int { return $this->id; }
    public function getNom(): ?string { return $this->nom; }
    public function setNom(?string $nom): static { $this->nom = $nom; return $this; }
    public function getUrlImage(): ?string { return $this->urlImage; }
    public function setUrlImage(?string $urlImage): static { $this->urlImage = $urlImage; return $this; }
    public function getDescription(): ?string { return $this->description; }
    public function setDescription(?string $description): static { $this->description = $description; return $this; }
    public function getPrixTotal(): ?float { return $this->prixTotal; }
    public function setPrixTotal(?float $prixTotal): static { $this->prixTotal = $prixTotal; return $this; }
    public function getEtatstock(): ?string { return $this->etatstock; }
    public function setEtatstock(?string $etatstock): static { $this->etatstock = $etatstock; return $this; }
    public function isArchive(): bool { return $this->etatstock === 'archiver'; }
    public function isDisponible(): bool { return $this->etatstock === 'disponible'; }
}
