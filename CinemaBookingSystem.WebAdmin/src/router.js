import { createRouter, createWebHistory }  from 'vue-router'
import { authService } from '@/auth'
import Home from '@/views/Home'
import Cinemas from '@/views/Cinemas'
import Cinema from '@/views/Cinema'
import Movies from '@/views/Movies'
import Movie from '@/views/Movie'
import Seances from '@/views/Seances'
import Bookings from '@/views/Bookings'
import HealthCheck from '@/views/HealthCheck'

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/cinemas',
    name: 'Cinemas',
    component: Cinemas
  },
  {
    path: '/cinema',
    name: 'Cinema',
    component: Cinema
  },
  {
    path: '/movies',
    name: 'Movies',
    component: Movies
  },
  {
    path: '/movies/new',
    name: 'Movie',
    component: Movie
  },
  {
    path: '/seances',
    name: 'Seances',
    component: Seances
  },
  {
    path: '/bookings',
    name: 'Bookings',
    component: Bookings
  },
  {
    path: '/hc',
    name: 'HealtCheck',
    component: HealthCheck
  }
]

const router = createRouter({
    history: createWebHistory(process.env.BASE_URL),
    routes: routes
  })
  
router.beforeEach((to, from, next) => {
  if (to.path === '/login') {
    authService.handleLoginRedirect()
      .then(() =>{
      })
      .catch(error => {
        console.log(error)
      })
      next({
        path: '/'
      })
      return 
  } else if (to.path === '/logout') {
    authService.handleLogoutRedirect()
      .then(() => {
      })
      .catch(error => {
        console.log(error)
      })
      next({
        path: '/'
      })
      return 
  }

  next()
  return
})
export default router
