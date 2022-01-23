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
            img-src="https://cdn.benchmark.pl/uploads/article/81190/MODERNICON/885df79e105d78b531580951338378647c14953a.jpg"
            >
            <div class="carousel-caption d-none d-md-block glassEffect">
                <h3>No time to die</h3>
                <b-button pill @click="onSeanceClicked()">Buy tickets now</b-button>
            </div></b-carousel-slide>
            <b-carousel-slide
            class="carousel-item"
            img-src="https://sm.ign.com/t/ign_pl/screenshot/default/ndnn_pt1c.1280.jpg"
            >
             <div class="carousel-caption d-none d-md-block glassEffect">
                <h3>House of Gucci</h3>
                <b-button pill @click="onSeanceClicked()">Buy tickets now</b-button>
            </div></b-carousel-slide>
            <b-carousel-slide
            class="carousel-item"
            img-src="https://images3.alphacoders.com/858/858193.jpg"
            >
             <div class="carousel-caption d-none d-md-block glassEffect">
                <h3>Kingsman</h3>
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
