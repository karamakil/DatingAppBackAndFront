import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { RolesModalComponent } from 'src/app/modals/roles-modal/roles-modal.component';
import { User } from 'src/app/_Models/User';
import { AdminService } from 'src/app/_Services/admin.service';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit {
  users: Partial<User[]>;
  modalRef?: BsModalRef;

  constructor(private adminService: AdminService
    , private modalService: BsModalService) { }

  ngOnInit(): void {
    this.GetUsersWithRoles();
  }

  GetUsersWithRoles() {
    this.adminService.GetUsersWithRoles().subscribe(u => {
      this.users = u
    })
  }

  OpenRolesModal(user) {
    const config = {
      class: 'modal-dialog-centered',
      initialState: {
        user,
        roles: this.GetRolesArray(user),
      }
    }
    this.modalRef = this.modalService.show(RolesModalComponent, config);


    this.modalRef.content.updateSelectedRoles.subscribe(values => {
      const rolesToUpdate = {
        roles: [...values.filter(el => el.checked == true).map(el => el.name)]
      }
      if (rolesToUpdate) {
        this.adminService.UpdateUserRoles(user.userName, rolesToUpdate.roles).subscribe(() => {
          user.roles = [...rolesToUpdate.roles];
        })
      }
    })
  }

  private GetRolesArray(user) {
    const roles = [];
    const userRoles = user.roles;
    const availableRoles: any[] = [
      { name: "Admin", value: "Admin" },
      { name: "Moderator", value: "Moderator" },
      { name: "Member", value: "Member" },
    ]

    availableRoles.forEach(role => {
      let isMatch = false;
      for (const userRole of userRoles) {
        if (role.name == userRole) {
          isMatch = true;
          role.checked = true;
          roles.push(role);
          break;
        }
      }

      if (!isMatch) {
        role.checked = false;
        roles.push(role);
      }

    })

    return roles;
  }

}
