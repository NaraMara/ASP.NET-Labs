Изученные темы:
<ol >
<li >Построение форм, состоящих из нескольких шагов</li>
<li >Хранение данных в памяти процесса</li>
<li >Логгирование</li>
<li >Элементы форм</li>
</ol>

Выполненные задачи
<ol data-sourcepos="12:1-26:0" dir="auto">
<li data-sourcepos="12:1-12:75">Ознакомиться с заготовленным проектом.</li>
<li data-sourcepos="13:1-26:0">В заготовленном проекте:
<ol data-sourcepos="14:4-26:0">
<li data-sourcepos="14:4-17:137">Определить, реализовать и зарегистрировать службу:
<ul data-sourcepos="15:7-17:137">
<li data-sourcepos="15:7-17:137">Служба регистрации пользователей - позволяет определить, есть ли пользователь с указанными данными (имя, дата рождения, пол) и регистрировать их.
<ul data-sourcepos="16:9-17:137">
<li data-sourcepos="16:9-16:196">Реализация службы должна быть Service Stub-ом - данные пользователей должны сохраняться в памяти процесса.</li>
<li data-sourcepos="17:9-17:137">В процессе работы служба должна логгировать совершаемые ей действия.</li>
</ul>
</li>
</ul>
</li>
<li data-sourcepos="18:4-23:162">Реализовать контроллер, который будет использовать эти службу для того, чтобы
<ul data-sourcepos="19:7-23:162">
<li data-sourcepos="19:7-19:106">Позволять пользователь пройти процедуру регистрации</li>
<li data-sourcepos="20:7-20:146">Первоначально запрашивать у пользователя Имя, Фамилию, Дату рождения и Пол.</li>
<li data-sourcepos="21:7-21:211">Проверять наличие такого пользователя в системе, и если он существует - запрашивать подтверждение продолжения</li>
<li data-sourcepos="22:7-22:94">На последнем этапе - запрашивать почту и пароль</li>
<li data-sourcepos="23:7-23:162">После завершения процедуры регистрации - отобразить отчет о результате регистрации</li>
</ul>
