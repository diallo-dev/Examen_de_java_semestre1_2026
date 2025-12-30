<?php
namespace App\Controller;

use App\Entity\Burger;
use App\Form\BurgerType;
use App\Repository\BurgerRepository;
use Doctrine\ORM\EntityManagerInterface;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\HttpFoundation\Response;
use Symfony\Component\Routing\Attribute\Route;

#[Route('/burger')]
class BurgerController extends AbstractController
{
    #[Route('/', name: 'burger_index')]
    public function index(BurgerRepository $repo): Response
    {
        $burgers = $repo->findAllNonArchive();
        return $this->render('burger/index.html.twig', [
            'burgers' => $burgers,
        ]);
    }

    #[Route('/{id}/edit', name: 'burger_edit')]
    public function edit(Request $request, Burger $burger, EntityManagerInterface $em): Response
    {
        $form = $this->createForm(BurgerType::class, $burger);
        $form->handleRequest($request);

        if ($form->isSubmitted() && $form->isValid()) {
            $em->flush();
            $this->addFlash('success', 'Burger modifié avec succès');
            return $this->redirectToRoute('burger_index');
        }

        return $this->render('burger/edit.html.twig', [
            'burger' => $burger,
            'form' => $form,
        ]);
    }

    #[Route('/{id}/archive', name: 'burger_archive', methods: ['POST'])]
    public function archive(Burger $burger, BurgerRepository $repo): Response
    {
        $repo->archiverBurger($burger->getId());
        $this->addFlash('success', 'Burger archivé');
        return $this->redirectToRoute('burger_index');
    }
}
