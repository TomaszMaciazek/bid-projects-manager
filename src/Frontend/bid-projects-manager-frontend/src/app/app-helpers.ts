import { HttpParams } from "@angular/common/http";

export function toParams(data: any){
    let httpParams = new HttpParams();
    Object.keys(data).forEach(function (key) {
        if(data[key] != null && data[key] != undefined){
            if(data[key] instanceof Array){
                data[key].forEach((element : any) => {
                    httpParams = httpParams.append(key, element);
                });
            }
            else{
                httpParams = httpParams.append(key, data[key]);
            }
        }
    });
    return httpParams;
}