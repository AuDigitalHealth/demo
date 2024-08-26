import { Component, OnInit } from '@angular/core';
import { UntypedFormBuilder, UntypedFormControl, UntypedFormGroup } from '@angular/forms';
import { tap } from 'rxjs/operators';
import { PortalClient, DocumentModel, IQuestionnaireListResponseModel } from 'src/app/core/services/demo-client.service';
import { SelectionModel } from "@angular/cdk/collections";
import { environment } from 'src/environments/environment';
import { base64ToArrayBuffer } from 'src/app/core/utils/base-64-to-array-buffer';

enum BarcodeFormState {
	None,
	Loading,
	Error,
	Empty,
	Done,
}

@Component({
	selector: 'adha-demo-select',
	templateUrl: './demoselect.component.html',
	styleUrls: ['./demoselect.component.scss'],
})
export class HomeComponent implements OnInit {
	patientTableColumns: string[] = ['select', 'given-name', 'family-name', 'gender', 'dob'];
	formGroup: UntypedFormGroup;
	questionnaires: IQuestionnaireListResponseModel[] = [];
	selectedQuestionnaire?: IQuestionnaireListResponseModel;

	showDownloadFhirPayload: boolean = false;
	showDownloadHl7Payload: boolean = false;

	constructor(private formBuilder: UntypedFormBuilder, private portalClient: PortalClient)  {
		this.formGroup = this.formBuilder.group({
			barcode: [''],
		});
	}

	getServiceRequest() {

	}

	clear() {
		this.showDownloadFhirPayload = false;
		this.showDownloadHl7Payload = false;
	}

	ngOnInit() {
		this.portalClient.getQuestionnaires()
			.pipe(
				tap(response => {
					this.questionnaires = response;
				})
			).subscribe();
	}

	getDocument() {

		this.portalClient.getDocument('x')
			.pipe(
				tap(response => {
					let attachments = response as DocumentModel;
					let blob = new Blob([base64ToArrayBuffer(attachments.data!)], {
						type: attachments.mimeType,
					});
					let dataUrl = window.URL.createObjectURL(blob);

					const link = document.createElement('a');
					link.href = dataUrl;
					link.download = attachments.title!;
					link.click();

				})
			).subscribe();
	}

	// launch() questionnaire: IQuestionnaireListResponseModel


}