<?php
namespace App\Controller;

use App\Repository\StatistiqueRepository;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\HttpFoundation\Response;
use Symfony\Component\Routing\Attribute\Route;

class DashboardController extends AbstractController
{
    #[Route('/', name: 'dashboard')]
    #[Route('/dashboard', name: 'dashboard_home')]
    public function index(StatistiqueRepository $statRepo): Response
    {
        $stats = [
            'commandesEnCours' => $statRepo->getCommandesEnCoursDuJour(),
            'commandesValidees' => $statRepo->getCommandesValidesDuJour(),
            'commandesAnnulees' => $statRepo->getCommandesAnnuleesDuJour(),
            'recettes' => $statRepo->getRecettesJournalieres(),
            'burgersPlusVendus' => $statRepo->getBurgersPlusVendusDuJour(5),
            'menusPlusVendus' => $statRepo->getMenusPlusVendusDuJour(5),
        ];

        return $this->render('dashboard/index.html.twig', [
            'stats' => $stats,
        ]);
    }
}
