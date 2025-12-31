<?php

namespace App\Entity;

use App\Repository\CommandeRepository;
use Doctrine\ORM\Mapping as ORM;

#[ORM\Entity(repositoryClass: CommandeRepository::class)]
#[ORM\Table(name: 'commande')]
class Commande
{
    #[ORM\Id]
    #[ORM\GeneratedValue]
    #[ORM\Column]
    private ?int $id = null;

    #[ORM\Column(type: 'date', nullable: true)]
    private ?\DateTimeInterface $date = null;

    #[ORM\Column(name: 'etat_cmd', length: 255, nullable: true)]
    private ?string $etatCmd = null;

    #[ORM\Column(name: 'montant_total', nullable: true)]
    private ?float $montantTotal = null;

    #[ORM\Column(name: 'lieu_consommation', length: 255, nullable: true)]
    private ?string $lieuConsommation = null;

    #[ORM\Column(name: 'frais_livraison', nullable: true)]
    private ?float $fraisLivraison = null;

    #[ORM\ManyToOne(targetEntity: Client::class)]
    #[ORM\JoinColumn(name: 'id_client', referencedColumnName: 'id_client')]
    private ?Client $client = null;

    #[ORM\ManyToOne(targetEntity: Zone::class)]
    #[ORM\JoinColumn(name: 'id_zone', referencedColumnName: 'id')]
    private ?Zone $zone = null;

    #[ORM\ManyToOne(targetEntity: Livreur::class)]
    #[ORM\JoinColumn(name: 'id_livreur', referencedColumnName: 'id_livreur')]
    private ?Livreur $livreur = null;

    public function getId(): ?int { return $this->id; }

    public function getDate(): ?\DateTimeInterface { return $this->date; }

    public function setDate(?\DateTimeInterface $date): static { $this->date = $date; return $this; }

    public function getEtatCmd(): ?string { return $this->etatCmd; }

    public function setEtatCmd(?string $etatCmd): static { $this->etatCmd = $etatCmd; return $this; }

    public function getMontantTotal(): ?float { return $this->montantTotal; }

    public function getLieuConsommation(): ?string { return $this->lieuConsommation; }

    public function getFraisLivraison(): ?float { return $this->fraisLivraison; }

    public function getIdClient(): ?int { return $this->client ? $this->client->getId() : null; }

    public function getIdZone(): ?int { return $this->zone ? $this->zone->getId() : null; }

    public function getIdLivreur(): ?int { return $this->livreur ? $this->livreur->getId() : null; }

    public function getClient(): ?Client { return $this->client; }

    public function getZone(): ?Zone { return $this->zone; }

    public function getLivreur(): ?Livreur { return $this->livreur; }
}