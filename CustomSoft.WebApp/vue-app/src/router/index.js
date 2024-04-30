import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import AddInvoicesToProveedorView from '../views/AddInvoicesToProveedorView.vue'
import EditFormProveedorView from '../views/EditFormProveedorView.vue'
import RegisterFormProveedorView from '../views/RegisterFormProveedorView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/edit-provider/:id',
        name: 'EditFormProveedorView',
        component: EditFormProveedorView,
        props: true
      },
      {
          path: '/register-provider',
          name: 'RegisterFormProveedorView',
          component: RegisterFormProveedorView,
      },
      {
          path: '/add-invoices-provider/:id',
          name: 'AddInvoicesToProveedorView',
          component: AddInvoicesToProveedorView,
          props: true
      }
  ]
})

export default router
