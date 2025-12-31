<?php

namespace App\Controller;

use App\Repository\CommandeRepository;
use App\Repository\LivreurRepository;
use App\Repository\ZoneRepository;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\HttpFoundation\Response;
use Symfony\Component\Routing\Attribute\Route;

#[Route('/livraison')]
class LivraisonController extends AbstractController
{
    
    #[Route('/zones', name: 'livraison_zones')]
    public function zones(ZoneRepository $zoneRepo): Response
    {
        $zones = $zoneRepo->findAllWithCommandesCount();
        
        return $this->render('livraison/zones.html.twig', [
            'zones' => $zones,
        ]);
    }

   
    #[Route('/zone/{id}', name: 'livraison_zone_details')]
    public function zoneDetails(
        int $id, 
        CommandeRepository $commandeRepo,
        LivreurRepository $livreurRepo,
        ZoneRepository $zoneRepo
    ): Response {
       
        $commandes = $commandeRepo->findByZone($id);
        
        
        $livreursObjets = $livreurRepo->findAll(); 
        $livreursData = [];

        foreach ($livreursObjets as $livreur) {
            $livreursData[] = [
                'id' => $livreur->getId(), 
                'nom' => $livreur->getNom(),
                'prenom' => $livreur->getPrenom(),
                'commandesEnCours' => $livreurRepo->countCommandesEnCours($livreur->getId())
            ];
        }

        
        $quartiers = $zoneRepo->getQuartiersByZone($id);
        
        return $this->render('livraison/zone_details.html.twig', [
            'zoneId' => $id,
            'commandes' => $commandes,
            'livreurs' => $livreursData,
            'quartiers' => $quartiers,
        ]);
    }

    
    #[Route('/affecter-multiple', name: 'livraison_affecter_multiple', methods: ['POST'])]
    public function affecterMultiple(
        Request $request,
        CommandeRepository $commandeRepo
    ): Response {
        $livreurId = $request->request->get('livreur_id');
        $commandeIds = $request->request->all('commande_ids');
        $zoneId = $request->request->get('zone_id');
        
        if (!$livreurId || empty($commandeIds)) {
            $this->addFlash('error', 'Veuillez sélectionner un livreur et au moins une commande');
            return $this->redirectToRoute('livraison_zone_details', ['id' => $zoneId]);
        }
        
        $count = 0;
        foreach ($commandeIds as $commandeId) {
            try {
                
                $commandeRepo->affecterLivreur((int)$commandeId, (int)$livreurId);
                $count++;
            } catch (\Exception $e) {
            }
        }
        
        if ($count > 0) {
            $this->addFlash('success', "$count commande(s) affectée(s) au livreur.");
        }
        
        return $this->redirectToRoute('livraison_zone_details', ['id' => $zoneId]);
    }
}