<?php
namespace App\Controller;

use App\Entity\Complement;
use App\Form\ComplementType;
use App\Repository\ComplementRepository;
use Doctrine\ORM\EntityManagerInterface;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\HttpFoundation\Response;
use Symfony\Component\Routing\Attribute\Route;

#[Route('/complement')]
class ComplementController extends AbstractController
{
    #[Route('/', name: 'complement_index')]
    public function index(ComplementRepository $repo): Response
    {
        $complements = $repo->findAllNonArchive();
        return $this->render('complement/index.html.twig', [
            'complements' => $complements,
        ]);
    }

    #[Route('/{id}/edit', name: 'complement_edit')]
    public function edit(Request $request, Complement $complement, EntityManagerInterface $em): Response
    {
        $form = $this->createForm(ComplementType::class, $complement);
        $form->handleRequest($request);

        if ($form->isSubmitted() && $form->isValid()) {
            $em->flush();
            $this->addFlash('success', 'Complément modifié avec succès');
            return $this->redirectToRoute('complement_index');
        }

        return $this->render('complement/edit.html.twig', [
            'complement' => $complement,
            'form' => $form,
        ]);
    }

    #[Route('/{id}/archive', name: 'complement_archive', methods: ['POST'])]
    public function archive(Complement $complement, ComplementRepository $repo): Response
    {
        $repo->archiverComplement($complement->getId());
        $this->addFlash('success', 'Complément archivé');
        return $this->redirectToRoute('complement_index');
    }
}
