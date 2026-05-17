package com.depi

/* groovylint-disable-next-line NoDef */
def packgeJar(packagejavaOpt) {
    sh " mvn clean packge install ${packagejavaOpt} "
}
def testJar(testjavaOpt) {
    sh " mvn clean packge install ${testjavaOpt} "
}
