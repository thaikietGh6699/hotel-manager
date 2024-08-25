using APP.Commands;
using APP.Data;
using APP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace APP.ViewModels
{
    internal class MemberViewModel 
    {
        private DatabaseHelper databaseHelper;
        public ObservableCollection<Member> Members { get; set; }   
        public ICommand LoadMembersCommand { get; set; }
        public ICommand AddMembersCommand { get; set; }
        public ICommand UpdateMembersCommand { get; set; }
        public ICommand DeleteMembersCommand { get; set; }
        public ICommand ExtendMembersCommand { get; set; }

        private Member _selectedMember;
        public Member SelectedMember
        {
            get => _selectedMember;
            set
            {
                _selectedMember = value;
                OnPropertyChanged();
            }
        }
        public MemberViewModel()
        {
            databaseHelper = new DatabaseHelper();
            Members = new ObservableCollection<Member>();
            LoadMembersCommand = new RelayCommand(LoadMembers);
            AddMembersCommand = new RelayCommand(AddMember);
            UpdateMembersCommand = new RelayCommand(UpdateMember);
            DeleteMembersCommand = new RelayCommand(DeleteMember);
            ExtendMembersCommand = new RelayCommand(ExtendMembership);
        }
        private void LoadMembers()
        {
            var members = databaseHelper.GetMembers();
            Members.Clear();
            foreach (var member in members)
            {
                Members.Add(member);
            }
        }
        private void AddMember()
        {
            if (SelectedMember != null)
            {
                databaseHelper.AddMember(SelectedMember);
                LoadMembers();
            }
        }

        private void UpdateMember()
        {
            if (SelectedMember != null)
            {
                databaseHelper.UpdateMember(SelectedMember);
                LoadMembers();
            }
        }

        private void DeleteMember()
        {
            if (SelectedMember != null)
            {
                databaseHelper.DeleteMember(SelectedMember.MemberID);
                LoadMembers();
            }
        }

        private void ExtendMembership()
        {
            if (SelectedMember != null)
            {
                databaseHelper.ExtendMembership(SelectedMember.MemberID);
                LoadMembers();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
