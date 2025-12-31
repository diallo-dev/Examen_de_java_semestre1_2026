<?php
namespace App\Controller;

use App\Entity\Commande;
use App\Repository\CommandeRepository;
use App\Repository\ClientRepository;
use App\Repository\LivreurRepository; 
use Doctrine\ORM\EntityManagerInterface;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\HttpFoundation\Response;
use Symfony\Component\Routing\Attribute\Route;

#[Route('/commande')]
class CommandeController extends AbstractController
{
    #[Route('/', name: 'commande_index')]
    public function index(Request $request, CommandeRepository $repo, ClientRepository $clientRepo): Response
    {
        $etat = $request->query->get('etat');
        $date = $request->query->get('date');
        $clientId = $request->query->get('client');

        if ($etat || $date || $clientId) {
            $commandes = $repo->findByFilters($etat, $date, $clientId);
        } else {
            $commandes = $repo->findAllWithDetails();
        }

        $clients = $clientRepo->findAll();

        return $this->render('commande/index.html.twig', [
            'commandes' => $commandes,
            'clients' => $clients,
            'filterEtat' => $etat,
            'filterDate' => $date,
            'filterClient' => $clientId,
        ]);
    }

    #[Route('/{id}', name: 'commande_show')]
    public function show(Commande $commande, CommandeRepository $repo, LivreurRepository $livreurRepo): Response
    {
        $items = $repo->getItemsByCommandeId($commande->getId());
        
        
        $livreurs = $livreurRepo->findAll();
        
        return $this->render('commande/show.html.twig', [
            'commande' => $commande,
            'items' => $items,
            'livreurs' => $livreurs, 
        ]);
    }

    #[Route('/{id}/terminer', name: 'commande_terminer', methods: ['POST'])]
    public function terminer(Commande $commande, CommandeRepository $repo): Response
    {
        $repo->terminerCommande($commande->getId());
        $this->addFlash('success', 'Commande marquée comme terminée');
        return $this->redirectToRoute('commande_index');
    }

    #[Route('/{id}/annuler', name: 'commande_annuler', methods: ['POST'])]
    public function annuler(Commande $commande, CommandeRepository $repo): Response
    {
        $repo->annulerCommande($commande->getId());
        $this->addFlash('success', 'Commande annulée');
        return $this->redirectToRoute('commande_index');
    }

    #[Route('/{id}/affecter-livreur', name: 'commande_affecter_livreur', methods: ['POST'])]
    public function affecterLivreur(Request $request, Commande $commande, CommandeRepository $repo): Response
    {
        $livreurId = $request->request->get('livreur_id');
        $repo->affecterLivreur($commande->getId(), $livreurId);
        $this->addFlash('success', 'Livreur affecté à la commande');
        return $this->redirectToRoute('commande_show', ['id' => $commande->getId()]);
    }

    #[Route('/zone/{id}', name: 'commande_par_zone')]
    public function parZone(int $id, CommandeRepository $repo): Response
    {
        $commandes = $repo->findByZone($id);
        
        return $this->render('commande/par_zone.html.twig', [
            'commandes' => $commandes,
            'zoneId' => $id,
        ]);
    }
}