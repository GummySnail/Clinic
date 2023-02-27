import { Component } from '@angular/core';
import {AuthService} from "../../services/auth.service";
import {Router} from "@angular/router";
import {MatDialog} from "@angular/material/dialog";
import {MatSnackBar, MatSnackBarConfig} from "@angular/material/snack-bar";
import {ConfirmationComponent} from "../../../shared/components/confirmation/confirmation.component";
import {TokenService} from "../../services/token.service";

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {
  constructor(public authService: AuthService, private tokenService: TokenService, private router: Router,
              private dialog: MatDialog, private snackBar: MatSnackBar) {}
  public logout(): void {
    this.dialog.open(ConfirmationComponent, { disableClose: true, autoFocus: false, data: { confirmationMessage: 'Confirm logout, please' } })
      .afterClosed().subscribe((result: boolean) => {
        if (!result){
          return;
        }
        this.tokenService.revokeToken().subscribe(() => {
          this.authService.deleteCurrentUser();
          let config = new MatSnackBarConfig();
          config.duration = 3000;
          this.snackBar.open('You successfully logout', 'Close', config);
          this.router.navigate(['']);
        });
    });
  }
}
