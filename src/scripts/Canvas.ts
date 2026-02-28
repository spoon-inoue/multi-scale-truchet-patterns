import * as THREE from 'three'
import { Layer } from './layer/Layer'
import { LayerMap } from './layer-map/LayerMap'

export class Canvas {
	private readonly renderer: THREE.WebGLRenderer
	private readonly clock: THREE.Clock
	private readonly layerMap: LayerMap
	private readonly layer: Layer

	constructor() {
		this.renderer = this.createRenderer()
		this.clock = new THREE.Clock()

		this.layerMap = new LayerMap(this.renderer)
		this.layer = new Layer(this.renderer, this.layerMap.texture)

		window.addEventListener('resize', this.resize.bind(this))
		this.renderer.setAnimationLoop(this.render.bind(this))
	}

	private get size() {
		const w = window.innerWidth
		const h = window.innerHeight
		return { width: w, height: h, aspect: w / h, dpr: window.devicePixelRatio }
	}

	private createRenderer() {
		const canvas = document.querySelector<HTMLCanvasElement>('canvas')!
		const renderer = new THREE.WebGLRenderer({ canvas })
		renderer.setSize(this.size.width, this.size.height)
		// renderer.setPixelRatio(this.size.dpr)
		renderer.setPixelRatio(2)
		return renderer
	}

	private resize() {
		this.renderer.setSize(this.size.width, this.size.height)
		this.layerMap.resize()
		this.layer.resize()
	}

	private render() {
		const dt = this.clock.getDelta()
		this.layerMap.render(dt)
		this.layer.render(dt)
	}
}
