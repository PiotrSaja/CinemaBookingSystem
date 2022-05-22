<template>
    <div>
        <b-carousel
            id="carousel-fade"
            style="text-shadow: 0px 0px 2px #000"
            fade
            indicators
        >
            <b-carousel-slide
            class="carousel-item"
            img-src="https://image.tmdb.org/t/p/original/5P8SmMzSNYikXpxil6BYzJ16611.jpg"
            >
            <div class="carousel-caption d-none d-md-block">
                <h1>The Batman</h1>
                <b-button pill @click="onSeanceClicked()">Buy tickets now</b-button>
            </div></b-carousel-slide>
            <b-carousel-slide
            class="carousel-item"
            img-src="https://image.tmdb.org/t/p/original/oQPbZ5e6J9fuAyv4Gl0mMZMIyPI.jpg"
            >
             <div class="carousel-caption d-none d-md-block">
                <h1>The King's Man</h1>
                <b-button pill @click="onSeanceClicked()">Buy tickets now</b-button>
            </div></b-carousel-slide>
            <b-carousel-slide
            class="carousel-item"
            img-src="https://image.tmdb.org/t/p/original/8Y43POKjjKDGI9MH89NW0NAzzp8.jpg"
            >
             <div class="carousel-caption d-none d-md-block">
                <h1>Free Guy</h1>
                <b-button pill @click="onSeanceClicked()">Buy tickets now</b-button>
            </div></b-carousel-slide>
        </b-carousel>
    </div>
</template>

<script>
export default {
  name: 'MovieCarousel',
  methods: {
    showSeanceDetail () {
    this.$router.push({name: 'Seances'})
    },
    onSeanceClicked () {
      this.showSeanceDetail()
    }
  }
}
</script>

<style scoped>
.carousel-item {
  max-height:500px!important ;
}
.glassEffect{
  background-color: #ffffff10;
  backdrop-filter: blur(12px);
  -webkit-backdrop-filter: blur(12px);
  border-radius: 10px;
}
h3 {
    color: white;
    font-weight: bold;
}
p {
    font-size: 24px;
}
</style>
