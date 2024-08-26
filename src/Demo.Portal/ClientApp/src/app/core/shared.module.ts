import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule, DatePipe } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

// Material
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSelectModule } from '@angular/material/select';
import { MatChipsModule } from '@angular/material/chips';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatRadioModule } from '@angular/material/radio';
import { MatDialogModule } from '@angular/material/dialog';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatTreeModule } from '@angular/material/tree';
import { MatTabsModule } from '@angular/material/tabs';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { MatBadgeModule } from '@angular/material/badge';
import { MatDividerModule } from '@angular/material/divider';
import { MatMenuModule } from '@angular/material/menu';

// Components
import { FooterComponent } from './components/footer/footer.component';
import { HeaderComponent } from './components/header/header.component';

@NgModule({
	imports: [
		CommonModule,
		RouterModule,
		FormsModule,
		ReactiveFormsModule,

		// Angular Material
		MatProgressSpinnerModule,
		MatButtonModule,
		MatInputModule,
		MatIconModule,
		MatCardModule,
		MatDividerModule,
		MatCheckboxModule,
		MatSelectModule,
		MatChipsModule,
		MatAutocompleteModule,
		MatRadioModule,
		MatDialogModule,
		MatExpansionModule,
		MatTreeModule,
		MatTabsModule,
		MatSnackBarModule,
		MatTooltipModule,
		MatSidenavModule,
		MatPaginatorModule,
		MatTableModule,
		MatBadgeModule,
		MatSortModule,
		MatMenuModule,
	],
	declarations: [
		// Components
		HeaderComponent,
		FooterComponent,
	],
	exports: [
		// Angular Modules
		ReactiveFormsModule,
		CommonModule,

		// Angular Material
		MatProgressSpinnerModule,
		MatButtonModule,
		MatInputModule,
		MatIconModule,
		MatCardModule,
		MatDividerModule,
		MatCheckboxModule,
		MatSelectModule,
		MatChipsModule,
		MatAutocompleteModule,
		MatRadioModule,
		MatDialogModule,
		MatExpansionModule,
		MatTreeModule,
		MatTabsModule,
		MatSnackBarModule,
		MatTooltipModule,
		MatSidenavModule,
		MatPaginatorModule,
		MatTableModule,
		MatBadgeModule,
		MatSortModule,
		MatMenuModule,

		// Components
		HeaderComponent,
		FooterComponent,
	],
	providers: [DatePipe],
})
export class SharedModule {}
