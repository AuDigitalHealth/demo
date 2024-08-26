import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule, routes } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';

// App modules
import { SharedModule } from './core/shared.module';
import { environment } from 'src/environments/environment';

// Services

// App components
import { AppComponent } from './app.component';
import { HomeComponent } from './features/demoselect/demoselect.component';
import { RouterModule } from '@angular/router';
import { Demo_FHIR_API_BASE_URL, PortalClient } from './core/services/demo-client.service';
import {FormsModule} from "@angular/forms";

@NgModule({
	schemas: [CUSTOM_ELEMENTS_SCHEMA],
	imports: [
		SharedModule,
		BrowserModule,
		AppRoutingModule,
		RouterModule.forRoot(routes, {scrollPositionRestoration: 'enabled'}),
		BrowserAnimationsModule,
		HttpClientModule,
		FormsModule,
	],
	declarations: [AppComponent, HomeComponent],
	providers: [
		PortalClient,
		{
			provide: Demo_FHIR_API_BASE_URL,
			useValue: environment.demoApiEndpoint,
		}		
	],
	bootstrap: [AppComponent],
})
export class AppModule {}
