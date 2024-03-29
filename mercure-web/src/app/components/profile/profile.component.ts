import {Component, OnInit} from '@angular/core';
import {UserModel} from 'src/app/models/UserModel';
import {UserService} from "../../services/user/user.service";
import {ParameterModel} from 'src/app/models/ParameterModel';
import {environment} from "../../../environments/environment";
import {IRoleModel} from "../../models/IRoleModel";
import {RoleService} from "../../services/role/role.service";

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  currentUser: UserModel = {
    userId: 0,
    firstName: 'Loading...',
    lastName: 'Loading...',
    email: 'Loading...',
    role: {roleId: 0, roleName: "Loading...", roleNumber: -1}
  };

  currentParameter: ParameterModel = {
    langue: 'fr',
    monnaie: 'chf',
    shipmentAdress: 'Rue de Genève 63, 1004 Lausanne'
  }

  roles?: IRoleModel[];
  users?: UserModel[];

  isUserLoading: boolean = true;
  isAllUsersLoading: boolean = false;
  isParametersLoading: boolean = false;
  isDevEnv: boolean = false;
  orderIsShow: boolean = false;
  isAdmin: boolean = false;
  roleIsShown: boolean = false;
  parameterIsShow: boolean = false;
  selectedRole: any;

  constructor(private userService: UserService, private roleService: RoleService) {
    this.isDevEnv = !environment.production;
  }

  profilShow() {
    this.orderIsShow = false;
    this.parameterIsShow = false;
    this.roleIsShown = false;
  }

  orderShow() {
    this.orderIsShow = true;
    this.parameterIsShow = false;
    this.roleIsShown = false;
  }

  roleShow() {
    this.orderIsShow = false;
    this.parameterIsShow = false;
    this.roleIsShown = true;

    this.userService.getAllUsers()
      .then(u => {
        // Permet de ne pas afficher l'utilisateur courant dans la liste des utilisateurs
        this.users = u.filter(u => u.userId !== this.currentUser.userId);
      })
      .catch(e => {
        if (!environment.production) {
          console.log(e);
        }
      })
      .finally(() => this.isAllUsersLoading = false);
  }

  parameterShow() {
    this.orderIsShow = false;
    this.parameterIsShow = true;
  }

  ngOnInit(): void {
    this.userService.getUser()
      .then(u => {
        this.currentUser = u;
        this.selectedRole = u.role.roleNumber;
        // ROLE 100 = ADMIN
        this.isAdmin = u.role.roleNumber === 100;
      })
      .finally(() => this.isUserLoading = false);

    if (!environment.production) {
      this.roleService.getRoles()
        .then(r => this.roles = r);
    }
  }

  protected readonly confirm = confirm;
  protected readonly console = console;

  updateRole() {

    if (environment.production) {
      return;
    }

    if (this.selectedRole !== this.currentUser.role.roleNumber) {
      this.roleService.setRoles(this.selectedRole)
        .then(r => {
          this.userService.getUser(true)
            .then(u => {
              this.currentUser = u;
              this.selectedRole = u.role.roleNumber;
              this.isAdmin = u.role.roleNumber === 100;
            })
            .finally(() => this.isUserLoading = false);

          alert((r as any).message)
        })
        .catch(e => {
          console.log(e);
        });
    }
  }

  updateUserRole(user: UserModel) {
    let roleNumber = user.role.roleNumber;
    let userId = user.userId;

    this.roleService.setRoleToUser(userId, roleNumber)
      .catch(e => {
        if (!environment.production) {
          console.log(e);
        }
      })
      .finally(() => console.log("Role updated for user " + userId));
  }
}


