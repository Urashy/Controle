using Front.Models;
using Front.Services;
using System.ComponentModel;

namespace Front.ViewModels
{
    public class AnimalViewModel : INotifyPropertyChanged
    {
        private readonly IAnimalService _animalService;
        private List<AnimalDTO> _animals = new();
        private List<AnimalDTO> _filteredAnimals = new();
        private string _searchText = string.Empty;
        private AnimalDTO? _selectedAnimal;
        private AnimalDTO _editAnimal = new();
        private bool _isEditing = false;
        private bool _isAdding = false;
        private string _errorMessage = string.Empty;

        public AnimalViewModel(IAnimalService animalService)
        {
            _animalService = animalService;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public List<AnimalDTO> FilteredAnimals
        {
            get => _filteredAnimals;
            set
            {
                _filteredAnimals = value;
                OnPropertyChanged(nameof(FilteredAnimals));
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        public AnimalDTO? SelectedAnimal
        {
            get => _selectedAnimal;
            set
            {
                _selectedAnimal = value;
                OnPropertyChanged(nameof(SelectedAnimal));
            }
        }

        public AnimalDTO EditAnimal
        {
            get => _editAnimal;
            set
            {
                _editAnimal = value;
                OnPropertyChanged(nameof(EditAnimal));
            }
        }

        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                _isEditing = value;
                OnPropertyChanged(nameof(IsEditing));
            }
        }

        public bool IsAdding
        {
            get => _isAdding;
            set
            {
                _isAdding = value;
                OnPropertyChanged(nameof(IsAdding));
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public async Task LoadAnimalsAsync()
        {
            try
            {
                _animals = await _animalService.GetAllAnimalsAsync();
                FilteredAnimals = new List<AnimalDTO>(_animals);
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement: {ex.Message}";
            }
        }

        public async Task SearchAnimalsByNameAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SearchText))
                {
                    await LoadAnimalsAsync();
                }
                else
                {
                    var result = await _animalService.GetAnimalByNameAsync(SearchText);
                    if (result != null)
                    {
                        FilteredAnimals = new List<AnimalDTO> { result };
                    }
                    else
                    {
                        FilteredAnimals = new List<AnimalDTO>();
                    }
                }
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la recherche: {ex.Message}";
            }
        }

        public void StartAdd()
        {
            EditAnimal = new AnimalDTO();
            IsAdding = true;
            IsEditing = false;
        }

        public void StartEdit(AnimalDTO animal)
        {
            EditAnimal = new AnimalDTO
            {
                Id = animal.Id,
                Name = animal.Name,
                Species = animal.Species,
                Poids = animal.Poids,
                Taille = animal.Taille
            };
            IsEditing = true;
            IsAdding = false;
        }

        public void CancelEdit()
        {
            IsEditing = false;
            IsAdding = false;
            EditAnimal = new AnimalDTO();
            ErrorMessage = string.Empty;
        }

        public async Task<bool> SaveAnimalAsync()
        {
            try
            {
                bool success;
                if (IsAdding)
                {
                    success = await _animalService.CreateAnimalAsync(EditAnimal);
                }
                else
                {
                    success = await _animalService.UpdateAnimalAsync(EditAnimal.Id, EditAnimal);
                }

                if (success)
                {
                    await LoadAnimalsAsync();
                    CancelEdit();
                    return true;
                }
                else
                {
                    ErrorMessage = "Erreur lors de la sauvegarde";
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur: {ex.Message}";
                return false;
            }
        }

        public async Task<bool> DeleteAnimalAsync(int id)
        {
            try
            {
                var success = await _animalService.DeleteAnimalAsync(id);
                if (success)
                {
                    await LoadAnimalsAsync();
                    return true;
                }
                else
                {
                    ErrorMessage = "Erreur lors de la suppression";
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur: {ex.Message}";
                return false;
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}