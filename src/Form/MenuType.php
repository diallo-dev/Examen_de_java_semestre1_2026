<?php
namespace App\Form;

use App\Entity\Menu;
use Symfony\Component\Form\AbstractType;
use Symfony\Component\Form\Extension\Core\Type\{NumberType, TextareaType, TextType, ChoiceType};
use Symfony\Component\Form\FormBuilderInterface;
use Symfony\Component\OptionsResolver\OptionsResolver;

class MenuType extends AbstractType
{
    public function buildForm(FormBuilderInterface $builder, array $options): void
    {
        $builder
            ->add('nom', TextType::class, ['label' => 'Nom'])
            ->add('prixTotal', NumberType::class, ['label' => 'Prix Total (FCFA)'])
            ->add('urlImage', TextType::class, ['label' => 'URL Image', 'required' => false])
            ->add('description', TextareaType::class, ['label' => 'Description', 'required' => false])
            ->add('etatstock', ChoiceType::class, [
                'label' => 'État',
                'choices' => [
                    'Disponible' => 'disponible',
                    'Indisponible' => 'indisponible',
                    'Archivé' => 'archiver',
                ],
            ]);
    }

    public function configureOptions(OptionsResolver $resolver): void
    {
        $resolver->setDefaults(['data_class' => Menu::class]);
    }
}
