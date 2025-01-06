import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChartSideComponent } from './chart-side.component';

describe('ChartSideComponent', () => {
  let component: ChartSideComponent;
  let fixture: ComponentFixture<ChartSideComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ChartSideComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChartSideComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
