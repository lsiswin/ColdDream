import { createRouter, createWebHistory } from 'vue-router';
import DefaultLayout from '../layouts/DefaultLayout.vue';
import Home from '../views/Home.vue';
import Login from '../views/Login.vue';
import Register from '../views/Register.vue';
import Tours from '../views/Tours.vue';
import TourDetail from '../views/TourDetail.vue';
import Booking from '../views/Booking.vue';
import CustomTourCreate from '../views/CustomTour/Create.vue';
import Dashboard from '../views/User/Dashboard.vue';
import OrderDetail from '../views/User/OrderDetail.vue';
import InspirationIndex from '../views/Inspiration/Index.vue';
import InspirationCreate from '../views/Inspiration/Create.vue';
import GuideIndex from '../views/Guide/Index.vue';
import GuideCreate from '../views/Guide/Create.vue';

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/',
      component: DefaultLayout,
      children: [
        { path: '', component: Home },
        { path: 'tours', component: Tours },
        { path: 'tours/:id', component: TourDetail },
        { path: 'booking/:id', component: Booking, meta: { requiresAuth: true } },
        { path: 'custom-tour', component: CustomTourCreate, meta: { requiresAuth: true } },
        { path: 'inspiration', component: InspirationIndex },
        { path: 'inspiration/create', component: InspirationCreate, meta: { requiresAuth: true } },
        { path: 'guide', component: GuideIndex },
        { path: 'guide/create', component: GuideCreate, meta: { requiresAuth: true } },
        { path: 'dashboard', component: Dashboard, meta: { requiresAuth: true } },
        { path: 'order/:id', component: OrderDetail, meta: { requiresAuth: true } },
      ]
    },
    {
      path: '/login',
      component: Login
    },
    {
      path: '/register',
      component: Register
    }
  ]
});

router.beforeEach((to, from, next) => {
  const token = localStorage.getItem('token');
  if (to.meta.requiresAuth && !token) {
    next('/login');
  } else {
    next();
  }
});

export default router;
