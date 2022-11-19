<template>
  <div>
  <b-navbar toggleable="lg" type="dark">
    <b-navbar-brand :to="{name: 'Home'}"  @click="activeElement = ''">
        <img src="@/assets/logo.png" class="d-inline-block align-top" alt="Logo" style="width: 36px;">
        MyCinema
    </b-navbar-brand>

    <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>

    <b-collapse id="nav-collapse" is-nav>
      <b-navbar-nav>
        <b-nav-item  v-bind:class="{active: activeElement === 'Seances' || activeElement === 'SeanceDetail'}" :to="{name: 'Seances'}">Seances</b-nav-item>
        <b-nav-item  v-bind:class="{active: activeElement === 'Cinemas'}" :to="{name: 'Cinemas'}">Cinemas</b-nav-item>
        <b-nav-item  v-bind:class="{active: activeElement === 'Movies' || activeElement === 'MovieDetail'}" :to="{name: 'Movies'}">Movies</b-nav-item>
      </b-navbar-nav>

      <!-- Right aligned nav items -->
      <b-navbar-nav class="ml-auto">
        <hr>
        <b-navbar-nav right>
        <b-nav-item v-if="!isUserLoggedIn" @click="onLogin()">Login</b-nav-item>
        <b-nav-item v-if="!isUserLoggedIn" href="https://saja.website:5001/Account/Register?ReturnUrl=https://saja.website">Sign-up</b-nav-item>
        <b-nav-item v-bind:class="{active: activeElement === 'Profile'}" v-if="isUserLoggedIn" to="/profile">{{ profile.email }}</b-nav-item>
        <b-nav-item v-if="isUserLoggedIn" @click="onLogout()">Logout</b-nav-item>
        </b-navbar-nav>
      </b-navbar-nav>
    </b-collapse>
  </b-navbar>
</div>
</template>

<script>
import { registerUserLoggedInEventListener, registerUserLoggedOutEventListener } from '../eventBus'
export default {
  name: 'Navbar',
  data () {
    return {
      activeElement: '',
      isUserLoggedIn: false,
      profile: null
    }
  },
  mounted () {
    this.$auth.isUserLoggedIn()
      .then(isLoggedIn => {
        this.isUserLoggedIn = isLoggedIn
      })
      .catch(error => {
        console.log(error)
        this.isUserLoggedIn = false
      })
    this.$auth.getProfile()
      .then(profile => {
        this.profile = profile
      })
      .catch(error => {
        console.log(error)
        this.profile = {}
      })
    registerUserLoggedInEventListener(() => { this.isUserLoggedIn = true })
    registerUserLoggedOutEventListener(() => { this.isUserLoggedIn = false })
  },
  methods: {
    onLogin () {
      this.$auth.login()
    },
    onLogout () {
      this.$auth.logout()
    }
  },
  computed: {
    currentRouteName () {
        return this.$route.name
    }
},
  created () {
    this.activeElement = this.currentRouteName
  }
}
</script>

<style scoped>
.navbar .navbar-nav .nav-link {
  color: white;
}
.navbar .navbar-nav .nav-link:hover {
  color: #FF9100;
}
.navbar-dark .navbar-nav .active>.nav-link, .navbar-dark .navbar-nav .nav-link.active, .navbar-dark .navbar-nav .nav-link.show, .navbar-dark .navbar-nav .show>.nav-link {
  color: #FF9100;
  font-weight: bold;
}
@media only screen and (min-width: 960px) {
  .navbar .navbar-nav .nav-link {
    padding: 1em 0.7em;
  }
  .navbar {
    padding: 0;
  }
  .navbar .navbar-brand {
    padding: 0 0.7em;
  }
}
.navbar .navbar-nav .nav-link {
  position: relative;
}
.navbar .navbar-nav .nav-link::after {
  position: absolute;
  bottom: 0;
  left: 0;
  right: 0;
  margin: auto;
  background-color: #FF9100;
  color: transparent;
  width: 0%;
  content: '';
  height: 3px;
  transition: all 0.5s;
}
.navbar .navbar-nav .nav-link:hover::after {
  width: 100%;
}
</style>
