import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { IonicModule } from "@ionic/angular";
import { AvatarComponent } from "./avatar.component";

@NgModule({
    imports: [
      CommonModule,
      IonicModule
    ],
    exports:[AvatarComponent],
    declarations: [AvatarComponent]
  })
  export class AvatarComponentModule {}