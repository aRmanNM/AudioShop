import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {TabsPage} from './tabs.page';

const routes: Routes = [
    {
        path: 'tabs',
        component: TabsPage,
        children: [
            {
                path: 'shop',
                loadChildren: () => import('../Shop/shop.module').then(m => m.ShopPageModule)
            },
            {
                path: 'user',
                loadChildren: () => import('../User/user.module').then(m => m.UserPageModule)
            },
            {
                path: 'basket',
                loadChildren: () => import('../Basket/basket.module').then(m => m.BasketPageModule)
            },
            {
                path: '',
                redirectTo: '/tabs/shop',
                pathMatch: 'full'
            }
        ]
    },
    {
        path: '',
        redirectTo: '/tabs/shop',
        pathMatch: 'full'
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class TabsPageRoutingModule {
}
