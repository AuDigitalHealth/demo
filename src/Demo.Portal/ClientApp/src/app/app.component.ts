import { AfterViewInit, Component } from '@angular/core';
import { MatIconRegistry } from '@angular/material/icon';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
	selector: 'adha-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.scss'],
})
export class AppComponent implements AfterViewInit {
	constructor(iconRegistry: MatIconRegistry, sanitizer: DomSanitizer) {
		iconRegistry.addSvgIcon('loader', sanitizer.bypassSecurityTrustResourceUrl('assets/loading.svg'));
	}

	ngAfterViewInit(): void {}
}
