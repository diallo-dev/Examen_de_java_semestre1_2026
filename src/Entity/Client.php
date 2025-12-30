<?php
namespace App\Entity;
use Doctrine\ORM\Mapping as ORM;

#[ORM\Entity]
#[ORM\Table(name: 'client')]
class Client
{
    #[ORM\Id, ORM\GeneratedValue, ORM\Column(name: 'id_client')]
    private ?int $id = null;
    #[ORM\Column(length: 255, nullable: true)]
    private ?string $nom = null;
    #[ORM\Column(length: 255, nullable: true)]
    private ?string $prenom = null;
    #[ORM\Column(length: 255, nullable: true)]
    private ?string $email = null;
    #[ORM\Column(length: 255, nullable: true)]
    private ?string $telephone = null;
    #[ORM\Column(length: 255, nullable: true)]
    private ?string $adresse = null;

    public function getId(): ?int { return $this->id; }
    public function getNom(): ?string { return $this->nom; }
    public function getPrenom(): ?string { return $this->prenom; }
    public function getEmail(): ?string { return $this->email; }
    public function getTelephone(): ?string { return $this->telephone; }
    public function getAdresse(): ?string { return $this->adresse; }
    public function getNomComplet(): string { return trim($this->prenom . ' ' . $this->nom); }
}
