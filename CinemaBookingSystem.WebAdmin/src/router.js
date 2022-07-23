import { createRouter, createWebHistory }  from 'vue-router'
import { authService } from '@/auth'
import Home from '@/views/Home'
import Cinemas from '@/views/Cinemas'
import Cinema from '@/views/Cinema'
import Movies from '@/views/Movies'
import Movie from '@/views/Movie'
import Seances from '@/views/Seances'
import Bookings from '@/views/Bookings'
import Booking from '@/views/Booking'
import HealthCheck from '@/views/HealthCheck'
import Profile from '@/views/Profile'
import Logout from '@/views/Logout'
import NotFound from '@/views/NotFound'
import NotAuth from '@/views/NotAuth'
import Welcome from '@/views/Welcome'

const routes = [
  {
    path: '/',
    name: 'Home',
    meta:{admin:true},
    component: Home
  },
  {
    path: '/cinemas',
    name: 'Cinemas',
    meta:{admin:true},
    component: Cinemas
  },
  {
    path: '/cinemas/:id',
    name: 'Cinema',
    meta:{admin:true},
    component: Cinema
  },
  {
    path: '/movies',
    name: 'Movies',
    meta:{admin:true},
    component: Movies
  },
  {
    path: '/movies/new',
    name: 'Movie',
    meta:{admin:true},
    component: Movie
  },
  {
    path: '/seances',
    name: 'Seances',
    meta:{admin:true},
    component: Seances
  },
  {
    path: '/bookings',
    name: 'Bookings',
    meta:{admin:true},
    component: Bookings
  },
  {
    path: '/bookings/:id',
    name: 'Booking',
    meta:{admin:true},
    component: Booking
  },
  {
    path: '/hc',
    name: 'HealhtCheck',
    meta:{admin:true},
    component: HealthCheck
  },
  {
    path: '/profile',
    name: 'Profile',
    meta:{admin:true},
    component: Profile
  },
  {
    path: '/profile/logout',
    name: 'Logout',
    component: Logout
  },
  {
    path: '/404',
    name: 'NotFound',
    component: NotFound,
    props: true
  },
  {
    path: '/401',
    name: 'NotAuth',
    component: NotAuth,
    props: true
  },
  {
    path: '/start',
    name: 'Welcome',
    component: Welcome,
    props: true
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
        path: '/start'
      })
      return 
  } else if (to.meta.admin && (to.path !== '/start' || to.path !== '/login')) {
    authService.isUserLoggedIn()
      .then(isLoggedIn => {
        if (isLoggedIn === true) {
          authService.getProfile()
          .then(profile => {
            const role = profile.role
    
            if(role !== 'Administrator')
              next({path:'/401'})
            else
              next()
          })
          .catch(error => {
            console.log(error)
            this.profile = {}
          })
        }
        else {
          next({path:'/401'})
        }
      })
      .catch(error => {
        console.log(error)
      })
      return
  }

  next()
  return
})
export default router
