import { User } from './../model/users';
import { Component, OnInit } from '@angular/core';
import { UsersService } from '../service/users.service';

@Component({
  selector: 'app-users',
  standalone: false,
  templateUrl: './users.component.html',
  styleUrl: './users.component.css',
})
export class UsersComponent implements OnInit {
  emptyGuid: string = '00000000-0000-0000-0000-000000000000';
  isLoading: boolean = false;
  isError: boolean = false;
  users: User[] = [];
  selectedUser?: User;
  filterUser: string = '';
  filterEmail: string = '';

  constructor(private userService: UsersService) {}

  onUserSelect(user: User) {
    this.selectedUser = user;
    console.log(user);
  }

  applyFilter() {
    this.loadUsers();
  }

  onUserUpdated(user: User) {
    this.isError = false;
    this.selectedUser = undefined;
    if (user.id === this.emptyGuid) this.saveNewUser(user);
    else this.updateUser(user);
  }

  onNewUserCreate() {
    this.selectedUser = {
      id: this.emptyGuid,
      fullName: '',
      email: '',
      role: 'User',
      createdAt: undefined,
    };
  }
  saveNewUser(user: User) {
    this.userService.createUser(user).subscribe({
      next: (result) => {
        if (result.success) {
          this.loadUsers();
        } else {
          this.isError = true;
          console.error(
            'Error during user creation. Error code: ' +
              result.errorCode +
              ', error message: ' +
              result.message
          );
        }
      },
      error: (err) => {
        this.isError = true;
        console.error(err);
      },
    });
  }

  updateUser(user: User) {
    this.userService.updateUser(user).subscribe({
      next: (result) => {
        if (result.success) {
          this.loadUsers();
        } else {
          this.isError = true;
          console.error(
            'Error during user update. Error code: ' +
              result.errorCode +
              ', error message: ' +
              result.message
          );
        }
      },
      error: (err) => {
        this.isError = true;
        console.error(err);
      },
    });
  }

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers() {
    this.isLoading = true;
    this.isError = false;
    this.userService.getusers(this.filterEmail, this.filterUser).subscribe({
      next: (result) => {
        if (result.success) {
          this.users = result.data;
          this.isLoading = false;
        } else {
          this.isError = true;
          this.isLoading = false;
          console.error(
            'Error during loading list of users. Error code: ' +
              result.errorCode +
              ', error message: ' +
              result.message
          );
        }
      },
      error: (err) => {
        this.isLoading = false;
        this.isError = true;
        console.error(err);
      },
    });
  }
  onCloseAddTask() {
    this.selectedUser = undefined;
  }

  confirmDelete(user: any) {
    const confirmation = confirm(
      `Are you sure what to delete user: ${user.fullName}?`
    );
    if (confirmation) {
      this.deleteUser(user.id);
    }
  }

  deleteUser(userId: string) {
    this.userService.deleteUser(userId).subscribe({
      next: (result) => {
        if (result.success) {
          this.loadUsers();
        } else {
          this.isError = true;
          console.error(
            'Error during user deletion. Error code: ' +
              result.errorCode +
              ', error message: ' +
              result.message
          );
        }
      },
      error: (err) => {
        this.isError = true;
        console.error(err);
      },
    });
  }
}
