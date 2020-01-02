import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-header',
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.less']
})
export class HeaderComponent implements OnInit {

    constructor() { }

    ngOnInit() {
        const closeNav = document.querySelector(".js-nav-close");
        const burgerMenu = document.querySelector(".js-menu-opener");
        const body = document.body;

        burgerMenu.addEventListener("click", function () {
            body.classList.add("nav--open");
        });

        closeNav.addEventListener("click", function () {
            body.classList.remove("nav--open");
        });
    }
}
