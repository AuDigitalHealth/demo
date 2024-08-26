import { Component } from '@angular/core';
import { environment } from '../../../../environments/environment';

@Component({
	selector: 'adha-footer',
	templateUrl: './footer.component.html',
	styleUrls: ['./footer.component.scss'],
})
export class FooterComponent {
	version = environment.version;
	created = environment.dateCreated;
}
