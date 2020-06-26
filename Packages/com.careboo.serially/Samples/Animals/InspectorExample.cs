using UnityEngine;

namespace CareBoo.Serially.Samples.Animals
{
    public class InspectorExample : MonoBehaviour
    {
        [DerivedFrom(typeof(IPet))]
        public SerializableType MyFavoriteTypeOfPet;

        [DerivedFrom(typeof(Dog))]
        public SerializableType MyFavoriteTypeOfDog;

        [ShowValueType]
        [SerializeReference]
        public IPet MyCurrentPet;

        public GameObject GameObjectReference;
    }
}
