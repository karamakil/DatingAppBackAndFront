import { HttpClient, HttpParams } from "@angular/common/http";
import { map } from "rxjs/operators";
import { PaginatedResult } from "../_Models/Pagination";

export function getPaginationResult<T>(url, params,http:HttpClient) {
    let paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();
    return http.get<T>(url, { observe: "response", params }).pipe(
      map(retVal => {
        paginatedResult.result = retVal.body;
        if (retVal.headers.get("Pagination") != null) {
          paginatedResult.pagination = JSON.parse(retVal.headers.get("Pagination"));
        }
        return paginatedResult;
      })
    );
  }

  export function GetPaginationHeaders(pageNumber: number, pageSize: number) {
    //httpparams is used to add query string to the url
    let params = new HttpParams();
    params = params.append("pageNumber", pageNumber.toString());
    params = params.append("pageSize", pageSize.toString());
    return params;
  }