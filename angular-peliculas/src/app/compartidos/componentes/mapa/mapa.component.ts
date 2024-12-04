import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import {
  icon,
  latLng,
  LeafletMouseEvent,
  marker,
  Marker,
  MarkerOptions,
  tileLayer,
} from 'leaflet';
import { LeafletModule } from '@bluehalo/ngx-leaflet';
import { Coordenada } from './Coordenada';
@Component({
  selector: 'app-mapa',
  standalone: true,
  imports: [LeafletModule],
  templateUrl: './mapa.component.html',
  styleUrl: './mapa.component.css',
})
export class MapaComponent implements OnInit{
  ngOnInit(): void {
    this.capas = this.coordenadasIniciales.map(valor => {
      const marcador = marker([valor.latitud, valor.longitud], this.markerOptions);

      return marcador
    })
  }
  @Input()
  coordenadasIniciales: Coordenada[] = [];

  @Output()
  coordenadaSeleccionada = new EventEmitter<Coordenada>();

  markerOptions: MarkerOptions = {
    icon: icon({
      iconSize: [25, 41],
      iconAnchor: [13, 41],
      iconUrl: 'assets/marker-icon-2x.png',
      iconRetinaUrl: 'assets/marker-icon-2x.png',
      shadowUrl: 'assets/marker-shadow.png',
    }),
  };

  options = {
    layers: [
      tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 18,
        attribution: '. . .',
      }),
    ],
    zoom: 14,
    center: latLng(-0.1745881528890485, -78.49287750427595),
  };

  capas: Marker<any>[] = [];
  manejarClick(event: LeafletMouseEvent) {
    const latitud = event.latlng.lat;
    const longitud = event.latlng.lng;

    this.capas = [];
    this.capas.push(marker([latitud, longitud], this.markerOptions));
    this.coordenadaSeleccionada.emit({ latitud, longitud });
  }
}
