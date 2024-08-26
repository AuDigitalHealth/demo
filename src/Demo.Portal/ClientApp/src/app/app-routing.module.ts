import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from './features/demoselect/demoselect.component';

export const routes: Routes = [
	{
		path: '',
		component: HomeComponent,
	},
	{
		path: 'home',
		redirectTo: '',
	},
	{
		path: '**',
		redirectTo: '',
	},
];

@NgModule({
	imports: [RouterModule.forRoot(routes)],
	exports: [RouterModule],
})
export class AppRoutingModule {}
