// route reuse strategy
// a way to cache components and not reloading them everytime path changes
// sources i learned about this:
// https://stackoverflow.com/questions/41280471/how-to-implement-routereusestrategy-shoulddetach-for-specific-routes-in-angular
// https://js.plainenglish.io/angular-route-reuse-strategy-b5d40adce841

import {ActivatedRouteSnapshot, DetachedRouteHandle, RouteReuseStrategy, UrlSegment} from '@angular/router';
import {LoginComponent} from '../auth/login/login.component';
import {RegisterComponent} from '../auth/register/register.component';

export class CustomRouteReuseStrategy implements RouteReuseStrategy {
  storedHandles: { [key: string]: DetachedRouteHandle } = {};

  shouldDetach(route: ActivatedRouteSnapshot): boolean {
    return route.data.reuseRoute || false;
  }

  store(route: ActivatedRouteSnapshot, handle: DetachedRouteHandle): void {
    const id = this.createIdentifier(route);
    if (route.data.reuseRoute) {
      this.storedHandles[id] = handle;
    }

    while (document.getElementsByTagName('mat-tooltip-component').length > 0) {
      document.getElementsByTagName('mat-tooltip-component')[0].remove();
    }
  }

  shouldAttach(route: ActivatedRouteSnapshot): boolean {
    const id = this.createIdentifier(route);
    const handle = this.storedHandles[id];
    const canAttach = !!route.routeConfig && !!handle;

    if (route.component === LoginComponent || route.component === RegisterComponent) {
      this.storedHandles = {};
      return false;
    }

    return canAttach;
  }

  retrieve(route: ActivatedRouteSnapshot): DetachedRouteHandle {
    const id = this.createIdentifier(route);
    if (!route.routeConfig || !this.storedHandles[id]) {
      return null;
    }
    return this.storedHandles[id];
  }

  shouldReuseRoute(future: ActivatedRouteSnapshot, curr: ActivatedRouteSnapshot): boolean {
    return future.routeConfig === curr.routeConfig;
  }

  private createIdentifier(route: ActivatedRouteSnapshot): any {
    // Build the complete path from the root to the input route
    const segments: UrlSegment[][] = route.pathFromRoot.map(r => r.url);
    const subpaths = ([] as UrlSegment[]).concat(...segments).map(segment => segment.path);
    // Result: ${route_depth}-${path}
    return segments.length + '-' + subpaths.join('/');
  }

  clear(): void {
    this.storedHandles = null;
  }
}
