import { Component, EventEmitter, Input, Output } from '@angular/core';
import { User } from '../model/users';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-edituser',
  standalone: false,
  templateUrl: './edituser.component.html',
  styleUrl: './edituser.component.css',
})
export class EdituserComponent {
  @Input() user!: User;
  @Output() userUpdated = new EventEmitter<User>();
  @Output() close = new EventEmitter<void>();
  editForm!: FormGroup;
  title: string = 'New user creation';

  constructor(private fb: FormBuilder) {}

  ngOnInit() {
    this.editForm = this.fb.group({
      fullName: [this.user.fullName, Validators.required],
      email: [this.user.email, [Validators.required, Validators.email]],
      role: [this.user.role, Validators.required],
    });
    if (this.user.id) {
      this.title = 'Edit user';
    } else {
      this.title = 'New user creation';
      this.user.role = 'User';
    }
  }

  onSubmit() {
    if (this.editForm.valid) {
      const updatedUser: User = {
        ...this.user,
        ...this.editForm.value,
      };
      this.userUpdated.emit(updatedUser);
    }
  }

  onCancel() {
    this.close.emit();
  }

  get fullName() {
    return this.editForm.get('fullName');
  }

  get email() {
    return this.editForm.get('email');
  }
}
