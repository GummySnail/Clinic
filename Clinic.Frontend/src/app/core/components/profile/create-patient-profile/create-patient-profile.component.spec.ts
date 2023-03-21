import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreatePatientProfileComponent } from './create-patient-profile.component';

describe('CreatePatientProfileComponent', () => {
  let component: CreatePatientProfileComponent;
  let fixture: ComponentFixture<CreatePatientProfileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreatePatientProfileComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreatePatientProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
