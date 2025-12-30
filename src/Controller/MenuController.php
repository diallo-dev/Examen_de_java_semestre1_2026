<?php
namespace App\Controller;

use App\Entity\Menu;
use App\Form\MenuType;
use App\Repository\MenuRepository;
use Doctrine\ORM\EntityManagerInterface;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\HttpFoundation\Response;
use Symfony\Component\Routing\Attribute\Route;

#[Route('/menu')]
class MenuController extends AbstractController
{
    #[Route('/', name: 'menu_index')]
    public function index(MenuRepository $repo): Response
    {
        $menus = $repo->findAllNonArchive();
        return $this->render('menu/index.html.twig', [
            'menus' => $menus,
        ]);
    }

    #[Route('/{id}/edit', name: 'menu_edit')]
    public function edit(Request $request, Menu $menu, EntityManagerInterface $em): Response
    {
        $form = $this->createForm(MenuType::class, $menu);
        $form->handleRequest($request);

        if ($form->isSubmitted() && $form->isValid()) {
            $em->flush();
            $this->addFlash('success', 'Menu modifié avec succès');
            return $this->redirectToRoute('menu_index');
        }

        return $this->render('menu/edit.html.twig', [
            'menu' => $menu,
            'form' => $form,
        ]);
    }

    #[Route('/{id}/archive', name: 'menu_archive', methods: ['POST'])]
    public function archive(Menu $menu, MenuRepository $repo): Response
    {
        $repo->archiverMenu($menu->getId());
        $this->addFlash('success', 'Menu archivé');
        return $this->redirectToRoute('menu_index');
    }
}
