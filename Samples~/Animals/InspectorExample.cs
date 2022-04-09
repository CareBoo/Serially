using UnityEngine;

namespace CareBoo.Serially.Samples.Animals
{
    public class InspectorExample : MonoBehaviour
    {
        [TypeFilter(typeof(IPet))]
        public SerializableType MyFavoriteTypeOfPet;

        [TypeFilter(typeof(Dog))]
        public SerializableType MyFavoriteTypeOfDog;

        [ShowSerializeReference]
        [SerializeReference]
        public IPet MyCurrentPet;

        public GameObject GameObjectReference;
    }
}
